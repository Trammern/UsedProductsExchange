import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {AuthenticationService} from './authentication.service';
import {Bid} from '../_models/bid';
import {Observable} from 'rxjs';
import {environment} from '../../environments/environment';
import {Product} from '../_models/product.model';

@Injectable({
  providedIn: 'root'
})
export class BidService {

  constructor(private http: HttpClient, private authenticationService: AuthenticationService) { }

  getBids():Observable<Bid[]>
  {
    let url = environment.apiUrl + '/bids'
    return this.http.get<Bid[]>(url);
  }

  //POST
  add(bid: Bid): Observable<Bid>{
    return this.http.post<Bid>(environment.apiUrl + '/bids', bid);
  }
}
