import { Component, OnInit } from '@angular/core';
import {Category} from '../_models/category';
import {CategoriesService} from '../_services/categories.service';
import {ProductsService} from '../_services/products.service';
import {catchError, tap} from 'rxjs/operators';
import {Observable} from 'rxjs';
import {Filter} from '../_models/filter';
import {FormBuilder, FormGroup} from '@angular/forms';
import {FilteredList} from '../_models/filtered-list';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  constructor(private productsService: ProductsService) { }

  ngOnInit(): void {
  }

}
