-- auto-generated definition
create table PaypalOrder
(
    orderID                varchar(255) charset utf8 not null
        primary key,
    billingToken           varchar(255) charset utf8 null,
    facilitatorAccessToken varchar(255) charset utf8 null,
    payerID                varchar(255) charset utf8 null,
    create_on              timestamp                 null
);
