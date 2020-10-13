using System.Linq.Expressions;
using System.Linq;
using dmc_auth.Entities;
using System;

namespace dmc_auth
{
  public enum OrderDir { ASC = 1, DESC = -1 };
  public abstract class PagingQuery<T>
  {
    public int offset { get; set; }
    public int limit { get; set; }
    public string orderBy { get; set; }
    public OrderDir orderDir { get; set; }
    public abstract Expression<Func<T, object>> getOrderExpression();
    public IQueryable<T> applyOrderQuery(IQueryable<T> query)
    {
      if (orderDir == OrderDir.ASC)
      {
        return query.OrderBy(getOrderExpression());
      }
      return query.OrderByDescending(getOrderExpression());
    }

    public IQueryable<T> applyPagingQuery(IQueryable<T> query)
    {
      return query.Skip(offset).Take(limit);
    }

    public IQueryable<T> BuildQuery(IQueryable<T> query)
    {
      query = applyPagingQuery(query);
      query = applyOrderQuery(query);
      return query;
    }
  }

  public class UserPagingQuery : PagingQuery<ApplicationUser>
  {
    public override Expression<Func<ApplicationUser, object>> getOrderExpression()
    {
      var lower = orderBy.ToLower();
      switch (lower)
      {
        case "username":
          return user => user.NormalizedUserName;
        case "email":
          return user => user.NormalizedEmail;
        default:
          return user => user.Id;
      }
    }
  }
}