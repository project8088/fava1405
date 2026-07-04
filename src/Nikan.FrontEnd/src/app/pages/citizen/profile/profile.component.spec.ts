import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenProfileComponent } from './profile.component';

describe('CitizenProfileComponent', () => {
  let component: CitizenProfileComponent;
  let fixture: ComponentFixture<CitizenProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CitizenProfileComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
