import { reducer, initialQueryState } from './query.reducer';

describe('Query Reducer', () => {
  describe('an unknown action', () => {
    it('should return the previous state', () => {
      const action = {} as any;

      const result = reducer(initialQueryState, action);

      expect(result).toBe(initialQueryState);
    });
  });
});
