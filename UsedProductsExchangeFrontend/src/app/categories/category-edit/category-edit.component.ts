import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {CategoriesService} from '../../_services/categories.service';
import {Category} from '../../_models/category';
import {catchError, switchMap, take, tap} from 'rxjs/operators';
import {Observable, of} from 'rxjs';
import {AuthenticationService} from '../../_services/authentication.service';

@Component({
  selector: 'app-category-edit',
  templateUrl: './category-edit.component.html',
  styleUrls: ['./category-edit.component.css']
})
export class CategoryEditComponent implements OnInit {
  updateObservable$: Observable<Category[]>;
  editCategoryForm: FormGroup;
  submitted = false;
  loading = false;
  errString: string;
  err: any;
  category: Category;

  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private categoryService: CategoriesService,
              private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    // this.activeRoute.params.subscribe(routeParams => {
    //   this.loadCategory(routeParams.id);
    // });

    this.updateObservable$ = this.activeRoute.paramMap
      .pipe(
        take(1),
        switchMap(params => {
          this.errString = '';
          const id = +params.get('id');
          return this.categoryService.getItem(id);
        }),
        tap(category => {
          this.editCategoryForm.patchValue(category);
          this.editCategoryForm.patchValue({
            categoryIdAfter: category.categoryId
          });
        }),
        catchError(this.err)
      );

    //  Initialize the form group
    this.editCategoryForm = this.formBuilder.group({
      name: ['', Validators.required],
      categoryId: [''],
      categoryIdAfter: [''],
    });
  }

  onSubmit(): void {
    this.submitted = true;

    // stop here if form is invalid
    if (this.editCategoryForm.invalid) {
      return;
    }

    this.loading = true;

    const updatedCategory = this.editCategoryForm.value;
    updatedCategory.categoryId = updatedCategory.categoryIdAfter;

    this.categoryService.update(updatedCategory)
      .pipe(
        catchError(err => {
          this.errString = err.error ?? err.message;
          return of();
        })
      )
      .subscribe(category => {
        console.log('category', category);
        this.router.navigateByUrl('categories');
      });
  }

  deleteCategory(): void {
    this.categoryService.remove(this.editCategoryForm.value.categoryId)
      .pipe(
        catchError(err => {
          this.errString = err.error;
          return err;
        })
      )
      .subscribe(bid => {
        this.router.navigateByUrl('/categories');
      });
  }
}
