import { Match } from './match';

export interface Bet {
  id: number,
  matchId: number,
  prediction: number,
  userId?: number,
  odds?: number,
  match?: Match
}
