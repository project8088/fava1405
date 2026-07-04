import { ComponentFixture, TestBed, async } from '@angular/core/testing';

import { CitizenProfileDialogComponent } from './citizen-profile.component';

describe('CitizenProfileDialogComponent', () => {
  let component: CitizenProfileDialogComponent;
  let fixture: ComponentFixture<CitizenProfileDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CitizenProfileDialogComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenProfileDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
