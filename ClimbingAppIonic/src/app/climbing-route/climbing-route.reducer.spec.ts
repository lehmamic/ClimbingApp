import { reducer, initialClimbingRouteState } from './climbing-route.reducer';

describe('ClimbingRoute Reducer', () => {
  describe('an unknown action', () => {
    it('should return the previous state', () => {
      const action = {} as any;

      const result = reducer(initialClimbingRouteState, action);

      expect(result).toBe(initialClimbingRouteState);
    });
  });
});
