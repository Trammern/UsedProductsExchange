import { Product } from '../_models/product.model';

export class Category {
  categoryId?: number;
  name: string;
  products?: Product[];
}
