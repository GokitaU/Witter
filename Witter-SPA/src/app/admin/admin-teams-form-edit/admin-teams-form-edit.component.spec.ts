import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTeamsFormEditComponent } from './admin-teams-form-edit.component';

describe('AdminTeamsFormEditComponent', () => {
  let component: AdminTeamsFormEditComponent;
  let fixture: ComponentFixture<AdminTeamsFormEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminTeamsFormEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTeamsFormEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
