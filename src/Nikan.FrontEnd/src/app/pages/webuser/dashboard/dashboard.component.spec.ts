import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuardDashboardComponent } from './dashboard.component';

describe('GuardDashboardComponent', () => {
  let component: GuardDashboardComponent;
  let fixture: ComponentFixture<GuardDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GuardDashboardComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GuardDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
