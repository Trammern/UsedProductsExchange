import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthenticationService } from './authentication.service';
import {Filter} from '../_models/filter';
import {FilteredList} from '../_models/filtered-list';
import { Product } from '../_models/product.model';
import {Category} from '../_models/category';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  currentProduct: Product;
  private deletedProduct: Observable<Product>;

  constructor(private http: HttpClient, private authenticationService: AuthenticationService) { }
  // GET
  getItems(filter: Filter): Observable<FilteredList<Product>> {
    let url = environment.apiUrl + '/products' + '?';
    if (filter && filter.itemsPrPage > 0 && filter.currentPage > 0) {
      url = url
        + 'itemsPrPage=' + filter.itemsPrPage
        + '&currentPage=' + filter.currentPage + '&';
    }
    if (filter && filter.searchField?.length > 0 && filter.searchText?.length > 0) {
      url = url
        + 'searchField=' + filter.searchField
        + '&searchText=' + filter.searchText;
    }
    return this.http.get<FilteredList<Product>>(url);
  }
  // GET
  getItem(id: number): Observable<Product> {
    // get product from api
    return this.http.get<Product>(environment.apiUrl + '/products/' + id);
  }

  update(productUpdated: Product): Observable<Product> {
    return this.http.put<Product>(environment.apiUrl + '/products/' + productUpdated.productId, productUpdated);
  }

  // DELETE
  remove(id: number): Observable<Product> {
    // get Products from api
    this.deletedProduct = this.http.delete<Product>(environment.apiUrl + '/products/' + id);
    return this.deletedProduct;
  }

  // POST
  add(product: Product): Observable<Product>{
    return this.http.post<Product>(environment.apiUrl + '/products', product);
  }

  setCurrentProduct(product: Product) {
    this.currentProduct = product;
  }

  GetCurrentProduct(): Product {
    return this.currentProduct;
  }
}
