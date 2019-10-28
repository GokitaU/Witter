import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LeagueListComponent } from './league-list.component';

describe('LeagueListComponent', () => {
  let component: LeagueListComponent;
  let fixture: ComponentFixture<LeagueListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LeagueListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LeagueListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
