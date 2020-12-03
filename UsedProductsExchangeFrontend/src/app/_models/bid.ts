import {User} from './user';

export class Bid {
  id: number;
  userId: number;
  productId: number;
  user: User;
  price: number;
  createdAt: any;
}
