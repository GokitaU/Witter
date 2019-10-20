import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Match } from '../_models/match';
import { Bet } from '../_models/bet';
import { BetService } from '../_services/bet.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-bet-form',
  templateUrl: './bet-form.component.html',
  styleUrls: ['./bet-form.component.css']
})
export class BetFormComponent implements OnInit {
  betForm: FormGroup;
  bet: Bet;
  prediction: number;
  @Input() match: Match;

  constructor(private fb: FormBuilder, private betService: BetService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.generateBetForm();
    this.getPrediction();
  }

  generateBetForm() {
    this.betForm = this.fb.group({
      prediction: ['1'],
      matchId: [this.match.id]
    });
  }

  getPrediction() {
    this.betService.getBet(this.match.id).subscribe((bet: Bet) => {
      console.log(bet);
      this.prediction = bet.prediction;
    });
  }

  placedBet() {
    if (this.prediction == undefined) {
      return false;
    }
    return true;
  }

  placeBet() {
    if (this.betForm.valid) {
      this.bet = Object.assign({}, this.betForm.value);

      this.betService.placeBet(this.bet).subscribe(() => {
        this.alertify.success("Placed bet successfully!");
      }, error => {
        this.alertify.error(error);
        }, () => {
          this.getPrediction();
        });
    }
  }

}
