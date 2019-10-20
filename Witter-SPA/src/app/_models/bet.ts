export interface Bet {
  id: number,
  matchId: number,
  prediction: number,
  userId?: number,
  odds?: number
}
