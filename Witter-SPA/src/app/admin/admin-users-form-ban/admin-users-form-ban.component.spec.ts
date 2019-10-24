import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUsersFormBanComponent } from './admin-users-form-ban.component';

describe('AdminUsersFormBanComponent', () => {
  let component: AdminUsersFormBanComponent;
  let fixture: ComponentFixture<AdminUsersFormBanComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminUsersFormBanComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUsersFormBanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
