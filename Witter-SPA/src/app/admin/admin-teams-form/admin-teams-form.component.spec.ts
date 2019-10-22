import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTeamsFormComponent } from './admin-teams-form.component';

describe('AdminTeamsFormComponent', () => {
  let component: AdminTeamsFormComponent;
  let fixture: ComponentFixture<AdminTeamsFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminTeamsFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTeamsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
