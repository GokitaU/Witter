import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminScoreFormComponent } from './admin-score-form.component';

describe('AdminScoreFormComponent', () => {
  let component: AdminScoreFormComponent;
  let fixture: ComponentFixture<AdminScoreFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminScoreFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminScoreFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
