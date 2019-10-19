export interface User {
  id: number;
  username: string;
  score?: number;
  isAdmin?: boolean;
  permanentBan?: boolean;
  ban?: Date;
}
