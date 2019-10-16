import { Component, OnInit } from '@angular/core';
import { AlertifyService } from './_services/alertify.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Witter-SPA';
  model: any = {};

  constructor(private alertify: AlertifyService) { }

  ngOnInit() {
    this.alertify.success('Alertify works :~~D');
  }
}
