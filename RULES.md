# Group standings rules used by the example

Based on the FIFA World Cup 2026 group stage rules (Article 13 of the FIFA regulations), simplified
for a test example.

## Points

- Win: 3
- Draw: 1
- Loss: 0

## Ranking

Teams are ordered by points. Teams level on points are separated by, in order:

1. Points in the matches between the tied teams (head-to-head)
2. Goal difference in those matches
3. Goals scored in those matches
4. Goal difference in all group matches
5. Goals scored in all group matches
6. Fair play score (higher is better):
   - yellow card: −1
   - red card after a second yellow: −3
   - straight red card: −4
7. Team name, alphabetically (stands in for the FIFA World Ranking, so the result is always
   deterministic)

## Simplifications

- The head-to-head steps are applied once to all tied teams. They are not re-applied to a
  smaller group that is still tied.
- No best-third-place ranking across groups.
