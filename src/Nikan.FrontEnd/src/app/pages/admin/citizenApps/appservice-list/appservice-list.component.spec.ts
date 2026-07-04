import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppserviceListComponent } from './appservice-list.component';

describe('AppserviceListComponent', () => {
  let component: AppserviceListComponent;
  let fixture: ComponentFixture<AppserviceListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppserviceListComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppserviceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
