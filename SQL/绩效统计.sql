select r.region_no, count(1)
  from T_PICK_LINE t, t_tag_locator r
 where t.locator = r.locator
   and t.last_update_date > to_date('2016-06-03', 'YYYY-MM-DD')
   and t.last_update_date < to_date('2016-09-04', 'YYYY-MM-DD')
 group by r.region_no;
 
 
 select t.last_update_by, count(1)
   from T_PICK_LINE t, t_tag_locator r
  where t.locator = r.locator
    and t.last_update_date > to_date('2016-06-03', 'YYYY-MM-DD')
    and t.last_update_date < to_date('2016-09-04', 'YYYY-MM-DD')
  group by t.last_update_by;
 
select  cdate(format('2009-10-22 12:22:23','yyyy-mm-dd'))  from dual;
 
 
 
 
