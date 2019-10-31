import { User } from './user';

export interface League {
  id: number,
  name: string,
  prize: string,
  admin: User,
  users?: User[],
  UserCount?: number,
  position?: number
}
