import { TestBed } from '@angular/core/testing';

import { BetService } from './bet.service';

describe('BetService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BetService = TestBed.get(BetService);
    expect(service).toBeTruthy();
  });
});
