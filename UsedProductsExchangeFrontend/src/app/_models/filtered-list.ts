import { Filter } from './filter';

export interface FilteredList<T>
{
  filter: Filter;
  totalCount: number;
  list: T[];
}
