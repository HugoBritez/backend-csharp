Select
IfNull(SUM(mcc.mc_debe),0.00) as saldo
From
movimientoscuentabco mcc Inner Join
movcuenta mc On mcc.mc_movimiento = mc.mc_codigo
 Where 1 = 1  and mc.mc_cuenta = 1 And mc.mc_fecha < '20250805'  And
mc.mc_estado = 1 and
mc.mc_tipo in(1,6)



