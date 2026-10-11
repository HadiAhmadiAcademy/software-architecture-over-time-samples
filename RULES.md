# Movie rental pricing rules used by the example

A rental is one movie, kept for a number of days. When the movie comes back, the customer is
charged once for the whole rental.

## Daily price

| Price code | Daily price | Allowed days |
|---|---|---|
| Regular | 2.00 | 2 |
| New release | 3.00 | no limit |
| Children | 1.50 | 3 |

## Late fee

Every day after the allowed days costs one and a half times the daily price.

## Examples

- Regular, 2 days: 2 × 2.00 = **4.00**
- Regular, 5 days: 2 × 2.00 + 3 × 3.00 = **13.00**
- New release, 5 days: 5 × 3.00 = **15.00**
- Children, 4 days: 3 × 1.50 + 1 × 2.25 = **6.75**

## Not covered

- What a rental of 0 days, or fewer, costs. The code accepts any number of days.
