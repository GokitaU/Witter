import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LeagueCreateFormComponent } from './league-create-form.component';

describe('LeagueCreateFormComponent', () => {
  let component: LeagueCreateFormComponent;
  let fixture: ComponentFixture<LeagueCreateFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LeagueCreateFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LeagueCreateFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
