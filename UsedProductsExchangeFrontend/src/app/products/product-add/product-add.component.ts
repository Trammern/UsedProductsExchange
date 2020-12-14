import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {ProductsService} from '../../_services/products.service';
import {Product} from '../../_models/product.model';
import {catchError, tap} from 'rxjs/operators';
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
  public progress: number;
  public message: string;
  @Output() public onUploadFinished = new EventEmitter();

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
      photo: [],
      price: ['', Validators.required],
      description: [''],
    });
  }

  get name() { return this.addProductForm.get('name'); }
  get description() { return this.addProductForm.get('description'); }
  get categoryId() { return this.addProductForm.get('categoryId'); }
  get expiration() { return this.addProductForm.get('expiration'); }
  get photo() { return this.addProductForm.get('photo'); }
  get price() { return this.addProductForm.get('price'); }

  onSubmit(): void {
    this.submitted = true;

    // stop here if form is invalid
    if (this.addProductForm.invalid) {
      return;
    }

    this.loading = true;

    const nameValue = this.name.value;
    const descriptionValue = this.description.value;
    const categoryValue = this.categoryId.value;
    const expirationValue = this.expiration.value;
    const photoValue = this.photo.value;
    const priceValue = this.price.value;

    const product: Product = {
      name: nameValue,
      bids: [],
      categoryId: categoryValue,
      currentPrice: priceValue,
      description: descriptionValue,
      expiration: expirationValue,
      pictureUrl: photoValue,
      userId: this.authenticationService.getUser().id,
    };

    this.productsService.add(product)
      .pipe(
        catchError(err => {
          this.loading = false;
          this.errormessage = err.error;
          return err;
        })
      )
      .subscribe(city => {
        this.name.reset();
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

  public uploadFile(files): void {
    if (files.length === 0) {
      return;
    }
    const fileToUpload = files[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    console.log(formData.getAll('file'));
    this.http.post(environment.apiUrl + '/upload', formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progress = Math.round(100 * event.loaded / event.total);
        }
        else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          this.onUploadFinished.emit(event.body);
        }
      });
  }
}
