select t1.wave_no,
       t1.flag_head,
       t1.order_no,
       t1.locator,
       t1.back_locator,
       t1.flag_line,
       t1.item_code,
       t1.item_barcode,
       t1.require_qty,
       t1.actual_qty,
       tr.com_id_ip,
       tr.com_id,
       tr.order_id_ip,
       tr.order_id,
       tr.aisle_lamp_id_ip,
       tr.aisle_lamp_id,
       tr.tag_id_ip,
       tr.tag_id
  from t_tag_locator tr,
       (select h.wave_no,
               h.flag_head,
               l.order_no,
               l.locator,
               l.back_locator,
               l.flag_line,
               d.item_code,
               d.item_barcode,
               d.require_qty,
               d.actual_qty
          from t_das_head h, t_das_line l, t_das_detail d
         where h.head_id = l.head_id
           and l.line_id = d.line_id
           and h.wave_no = 'w00003') t1
 where tr.locator = t1.locator
