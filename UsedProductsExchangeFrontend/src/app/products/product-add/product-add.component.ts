import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {ProductsService} from '../../_services/products.service';
import {Product} from '../../_models/product.model';
import {catchError, map, tap} from 'rxjs/operators';
import {AuthenticationService} from '../../_services/authentication.service';
import {Observable} from 'rxjs';
import {FilteredList} from '../../_models/filtered-list';
import {Category} from '../../_models/category';
import {Filter} from '../../_models/filter';
import {CategoriesService} from '../../_services/categories.service';
import {HttpClient, HttpEventType} from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-product-add',
  templateUrl: './product-add.component.html',
  styleUrls: ['./product-add.component.css']
})
export class ProductAddComponent implements OnInit {
  filterForm: FormGroup;
  addProductForm: FormGroup;
  submitted = false;
  loading = false;
  errormessage = '';
  listData$: Observable<FilteredList<Category>>;
  categories: Category[];
  count: number;
  err: any;
  filter: Filter = {
    itemsPrPage: 5,
    currentPage: 1
  };
  public product: Product;
  public response: {dbPath: ''};

  constructor(private http: HttpClient,
              private formBuilder: FormBuilder,
              private router: Router,
              private productsService: ProductsService,
              private categoriesService: CategoriesService,
              private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    this.filterForm = this.formBuilder.group({
      itemsPrPage: [''],
      currentPage: [''],
      searchText: ['']
    });
    this.filterForm.patchValue(this.filter);
    this.getCategories();

    //  Initialize the form group
    this.addProductForm = this.formBuilder.group({
      name: ['', Validators.required],
      expiration: ['', Validators.required],
      categoryId: ['', Validators.required],
      price: ['', Validators.required],
      description: [''],
    });
  }

  onSubmit(): void {
    console.log('Submitted');
    this.submitted = true;

    // stop here if form is invalid
    if (this.addProductForm.invalid) {
      return;
    }

    console.log('Form passed');

    this.loading = true;

    const nameValue = this.addProductForm.get('name').value;
    const descriptionValue = this.addProductForm.get('description').value;
    const categoryValue = this.addProductForm.get('categoryId').value;
    const expirationValue = this.addProductForm.get('expiration').value;
    const photoValue = this.response.dbPath;
    console.log(photoValue);
    const priceValue = this.addProductForm.get('price').value;

    this.product = {
      name: nameValue,
      bids: [],
      categoryId: categoryValue,
      currentPrice: priceValue,
      description: descriptionValue,
      expiration: expirationValue,
      pictureUrl: photoValue,
      userId: this.authenticationService.getUser().userId,
    };

    this.productsService.add(this.product)
      .pipe(map(response => {
        const product: Product = response;
        this.router.navigateByUrl('/products/' + product.productId);
      }))
      .pipe(
        catchError(err => {
          this.loading = false;
          this.errormessage = err.error;
          return err;
        })
      )
      .subscribe(() => {
        this.addProductForm.reset();
        this.loading = false;
        this.errormessage = '';
      });
  }

  getCategories(currentPage: number = 0): void {
    if (currentPage > 0) {
      this.filterForm.patchValue({ currentPage });
    }
    const filter = this.filterForm.value as Filter;
    if (filter.currentPage <= 0) {
      filter.currentPage = 1;
    }
    if (filter.searchText) {
      filter.searchField = 'Name';
    }
    this.listData$ = this.categoriesService.getItems(filter).pipe(
      tap(filteredList => {
        this.count = filteredList.totalCount;
        this.categories = filteredList.list;
      }),
      catchError(this.err)
    );
  }

  uploadFinished($event: any): void {
    this.response = $event;
  }

  public createImgPath = (serverPath: string) => {
    return environment.url + '/' + serverPath;
  }
}
