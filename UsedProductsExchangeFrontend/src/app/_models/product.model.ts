import { Bid } from '../_models/bid';
import {User} from './user';

export class Product {
  productId?: number;
  userId: number;
  name: string;
  description: string;
  pictureUrl: string;
  currentPrice: number;
  expiration: any;
  categoryId: number;
  bids: Bid[];
  user?: User;
}
