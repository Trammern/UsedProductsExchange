import { Bid } from '../_models/bid';

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
}
