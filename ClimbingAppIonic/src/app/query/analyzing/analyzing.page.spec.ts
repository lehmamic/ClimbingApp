import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AnalyzingPage } from './analyzing.page';

describe('AnalyzingPage', () => {
  let component: AnalyzingPage;
  let fixture: ComponentFixture<AnalyzingPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AnalyzingPage ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AnalyzingPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
