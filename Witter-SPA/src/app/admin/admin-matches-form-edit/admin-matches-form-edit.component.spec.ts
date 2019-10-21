import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminMatchesFormEditComponent } from './admin-matches-form-edit.component';

describe('AdminMatchesFormEditComponent', () => {
  let component: AdminMatchesFormEditComponent;
  let fixture: ComponentFixture<AdminMatchesFormEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminMatchesFormEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminMatchesFormEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
