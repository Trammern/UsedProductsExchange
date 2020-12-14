import {User} from './user';
import {Product} from './product';

export class Bid {
  bidId: number;
  userId: number;
  productId: number;
  product: Product;
  user: User;
  price: number;
  createdAt: any;
}
