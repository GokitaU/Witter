import { Team } from './team';
import { Score } from './score';

export interface Match {
  id: number;
  teamA: Team;
  teamB: Team;
  date: Date;
  teamAOdds: number;
  teamBOdds: number;
  drawOdds: number;
  score?: Score;
}
