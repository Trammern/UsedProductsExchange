import { Component, OnInit } from '@angular/core';
import {ProductsService} from '../../_services/products.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {catchError, switchMap, take, tap} from 'rxjs/operators';
import {Observable, of} from 'rxjs';
import {Product} from '../../_models/product.model';
import {ActivatedRoute, Router} from '@angular/router';
import {FilteredList} from '../../_models/filtered-list';
import {Category} from '../../_models/category';
import {Filter} from '../../_models/filter';
import {CategoriesService} from '../../_services/categories.service';
import {AuthenticationService} from '../../_services/authentication.service';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css']
})
export class ProductEditComponent implements OnInit {
  filterForm: FormGroup;
  updateObservable$: Observable<Product[]>;
  editProductForm: FormGroup;
  filter: Filter = {
    itemsPrPage: 5,
    currentPage: 1
  };
  count: number;
  submitted = false;
  loading = false;
  errString: string;
  err: any;
  listData$: Observable<FilteredList<Category>>;
  categories: Category[];
  product: Product;
  public response: {dbPath: ''};
  public newImage: boolean;

  constructor(private formBuilder: FormBuilder,
              private categoriesService: CategoriesService,
              private authenticationService: AuthenticationService,
              private router: Router,
              private productsService: ProductsService,
              private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.response = {dbPath: ''};
    this.filterForm = this.formBuilder.group({
      itemsPrPage: [''],
      currentPage: [''],
      searchText: ['']
    });
    this.filterForm.patchValue(this.filter);
    this.getCategories();
    this.updateObservable$ = this.activeRoute.paramMap
      .pipe(
        take(1),
        switchMap(params => {
          this.errString = '';
          const id = +params.get('id');
          return this.productsService.getItem(id);
        }),
        tap(product => {
          this.product = product;
          this.editProductForm.patchValue(product);
          this.editProductForm.patchValue({
            productIdAfter: product.productId
          });
        }),
        catchError(this.err)
      );

    //  Initialize the form group
    this.editProductForm = this.formBuilder.group({
      productId: [''],
      productIdAfter: [''],
      name: ['', Validators.required],
      expiration: ['', Validators.required],
      categoryId: ['', Validators.required],
      currentPrice: ['', Validators.required],
      description: [''],
    });
  }

  onSubmit(): void{
    this.submitted = true;

    // stop here if form is invalid
    if (this.editProductForm.invalid) {
      return;
    }

    this.loading = true;

    const updatedProduct = this.editProductForm.value;
    if (this.response.dbPath !== '' && this.response.dbPath !== undefined) {
      updatedProduct.pictureUrl = this.response.dbPath;
    } else {
      updatedProduct.pictureUrl = this.product.pictureUrl;
    }
    updatedProduct.productId = updatedProduct.productIdAfter;
    updatedProduct.userId = this.authenticationService.getUser().userId;

    console.log('product', updatedProduct);

    this.productsService.update(updatedProduct)
      .pipe(
        catchError(err => {
          this.errString = err.error ?? err.message;
          return of();
        })
      )
      .subscribe(product => {
        console.log('product', product);
        this.router.navigateByUrl('products');
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
}
