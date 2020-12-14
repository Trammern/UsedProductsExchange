import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { CategoriesService } from '../../_services/categories.service';
import {Category} from '../../_models/category';
import {catchError} from 'rxjs/operators';
import {HttpClient, HttpEventType} from '@angular/common/http';

@Component({
  selector: 'app-category-add',
  templateUrl: './category-add.component.html',
  styleUrls: ['./category-add.component.css']
})
export class CategoryAddComponent implements OnInit {
  addCategoryForm: FormGroup;
  submitted = false;
  loading = false;
  errormessage = '';
  @Output() public onUploadFinished = new EventEmitter();

  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private categoryService: CategoriesService) { }

  ngOnInit(): void {
    //  Initialize the form group
    this.addCategoryForm = this.formBuilder.group({
      name: ['', Validators.required],
    });
  }

  get name() { return this.addCategoryForm.get('name'); }

  onSubmit(): void {
    this.submitted = true;

    // stop here if form is invalid
    if (this.addCategoryForm.invalid) {
      return;
    }

    this.loading = true;

    const nameValue = this.name.value;

    const category: Category = {
      name: nameValue,
    };

    this.categoryService.add(category)
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
        // this.cityJustCreated = city;
      });
  }

}
