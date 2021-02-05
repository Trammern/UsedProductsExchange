import {Product} from './product.model';
import {Bid} from './bid';

export class User {
  userId?: number;
  name?: string;
  username?: string;
  password?: string;
  isAdmin?: boolean;
  address?: string;
  email?: string;
  products?: Product[];
  bids?: Bid[];
}
