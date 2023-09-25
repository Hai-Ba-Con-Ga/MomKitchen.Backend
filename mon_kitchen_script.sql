-- DROP SCHEMA public;

CREATE SCHEMA public AUTHORIZATION pg_database_owner;
-- public."__EFMigrationsHistory" definition

-- Drop table

-- DROP TABLE public."__EFMigrationsHistory";

CREATE TABLE public."__EFMigrationsHistory" (
	migration_id varchar(150) NOT NULL,
	product_version varchar(32) NOT NULL,
	CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);


-- public.area definition

-- Drop table

-- DROP TABLE public.area;

CREATE TABLE public.area (
	id uuid NOT NULL,
	"name" varchar(50) NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_area PRIMARY KEY (id)
);


-- public.dish definition

-- Drop table

-- DROP TABLE public.dish;

CREATE TABLE public.dish (
	id uuid NOT NULL,
	"name" varchar(50) NOT NULL,
	price float8 NOT NULL,
	image_url varchar(255) NOT NULL,
	description varchar(500) NULL,
	status text NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_dish PRIMARY KEY (id)
);


-- public.meal definition

-- Drop table

-- DROP TABLE public.meal;

CREATE TABLE public.meal (
	id uuid NOT NULL,
	"name" varchar(50) NOT NULL,
	price float8 NOT NULL,
	service_from timestamptz NOT NULL,
	service_to timestamptz NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_meal PRIMARY KEY (id)
);


-- public.promotion definition

-- Drop table

-- DROP TABLE public.promotion;

CREATE TABLE public.promotion (
	id uuid NOT NULL,
	"name" varchar(100) NOT NULL,
	amount int4 NOT NULL,
	start_date timestamptz NOT NULL,
	end_date timestamptz NOT NULL,
	description varchar(500) NULL,
	status text NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_promotion PRIMARY KEY (id)
);


-- public.province definition

-- Drop table

-- DROP TABLE public.province;

CREATE TABLE public.province (
	id uuid NOT NULL,
	"no" varchar(10) NOT NULL,
	"name" varchar(50) NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_province PRIMARY KEY (id)
);


-- public."role" definition

-- Drop table

-- DROP TABLE public."role";

CREATE TABLE public."role" (
	id uuid NOT NULL,
	"name" varchar(10) NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_role PRIMARY KEY (id)
);


-- public.area_meal definition

-- Drop table

-- DROP TABLE public.area_meal;

CREATE TABLE public.area_meal (
	areas_id uuid NOT NULL,
	meals_id uuid NOT NULL,
	CONSTRAINT pk_area_meal PRIMARY KEY (areas_id, meals_id),
	CONSTRAINT fk_area_meal_area_areas_id FOREIGN KEY (areas_id) REFERENCES public.area(id) ON DELETE CASCADE,
	CONSTRAINT fk_area_meal_meal_meals_id FOREIGN KEY (meals_id) REFERENCES public.meal(id) ON DELETE CASCADE
);
CREATE INDEX ix_area_meal_meals_id ON public.area_meal USING btree (meals_id);


-- public.dish_meal definition

-- Drop table

-- DROP TABLE public.dish_meal;

CREATE TABLE public.dish_meal (
	dishes_id uuid NOT NULL,
	meals_id uuid NOT NULL,
	CONSTRAINT pk_dish_meal PRIMARY KEY (dishes_id, meals_id),
	CONSTRAINT fk_dish_meal_dish_dishes_id FOREIGN KEY (dishes_id) REFERENCES public.dish(id) ON DELETE CASCADE,
	CONSTRAINT fk_dish_meal_meal_meals_id FOREIGN KEY (meals_id) REFERENCES public.meal(id) ON DELETE CASCADE
);
CREATE INDEX ix_dish_meal_meals_id ON public.dish_meal USING btree (meals_id);


-- public.district definition

-- Drop table

-- DROP TABLE public.district;

CREATE TABLE public.district (
	id uuid NOT NULL,
	"name" varchar(50) NOT NULL,
	province_id uuid NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_district PRIMARY KEY (id),
	CONSTRAINT fk_district_province_province_id FOREIGN KEY (province_id) REFERENCES public.province(id) ON DELETE CASCADE
);
CREATE INDEX ix_district_province_id ON public.district USING btree (province_id);


-- public."user" definition

-- Drop table

-- DROP TABLE public."user";

CREATE TABLE public."user" (
	id uuid NOT NULL,
	email varchar(50) NOT NULL,
	"password" varchar(20) NOT NULL,
	phone varchar(20) NOT NULL,
	birthday timestamptz NULL,
	avatar_url varchar(255) NULL,
	full_name varchar(50) NOT NULL,
	role_id uuid NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_user PRIMARY KEY (id),
	CONSTRAINT fk_user_role_role_id FOREIGN KEY (role_id) REFERENCES public."role"(id)
);
CREATE INDEX ix_user_role_id ON public."user" USING btree (role_id);


-- public.voucher definition

-- Drop table

-- DROP TABLE public.voucher;

CREATE TABLE public.voucher (
	id uuid NOT NULL,
	code varchar(10) NOT NULL,
	discount float8 NOT NULL,
	status text NOT NULL,
	promotion_id uuid NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_voucher PRIMARY KEY (id),
	CONSTRAINT fk_voucher_promotion_promotion_id FOREIGN KEY (promotion_id) REFERENCES public.promotion(id) ON DELETE CASCADE
);
CREATE INDEX ix_voucher_promotion_id ON public.voucher USING btree (promotion_id);


-- public.ward definition

-- Drop table

-- DROP TABLE public.ward;

CREATE TABLE public.ward (
	id uuid NOT NULL,
	"name" text NOT NULL,
	district_id uuid NOT NULL,
	province_id uuid NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_ward PRIMARY KEY (id),
	CONSTRAINT fk_ward_district_district_id FOREIGN KEY (district_id) REFERENCES public.district(id) ON DELETE CASCADE,
	CONSTRAINT fk_ward_province_province_id FOREIGN KEY (province_id) REFERENCES public.province(id) ON DELETE CASCADE
);
CREATE INDEX ix_ward_district_id ON public.ward USING btree (district_id);
CREATE INDEX ix_ward_province_id ON public.ward USING btree (province_id);


-- public.customer definition

-- Drop table

-- DROP TABLE public.customer;

CREATE TABLE public.customer (
	id uuid NOT NULL,
	point_wallet int4 NOT NULL,
	user_id uuid NOT NULL,
	status text NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_customer PRIMARY KEY (id),
	CONSTRAINT fk_customer_user_user_id FOREIGN KEY (user_id) REFERENCES public."user"(id) ON DELETE CASCADE
);
CREATE UNIQUE INDEX ix_customer_user_id ON public.customer USING btree (user_id);


-- public.kitchen definition

-- Drop table

-- DROP TABLE public.kitchen;

CREATE TABLE public.kitchen (
	id uuid NOT NULL,
	"name" varchar(50) NOT NULL,
	status text NOT NULL,
	owner_id uuid NOT NULL,
	province_id uuid NOT NULL,
	district_id uuid NOT NULL,
	ward_id uuid NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_kitchen PRIMARY KEY (id),
	CONSTRAINT fk_kitchen_district_district_id FOREIGN KEY (district_id) REFERENCES public.district(id) ON DELETE CASCADE,
	CONSTRAINT fk_kitchen_province_province_id FOREIGN KEY (province_id) REFERENCES public.province(id) ON DELETE CASCADE,
	CONSTRAINT fk_kitchen_user_owner_id FOREIGN KEY (owner_id) REFERENCES public."user"(id) ON DELETE CASCADE,
	CONSTRAINT fk_kitchen_ward_ward_id FOREIGN KEY (ward_id) REFERENCES public.ward(id) ON DELETE CASCADE
);
CREATE INDEX ix_kitchen_district_id ON public.kitchen USING btree (district_id);
CREATE INDEX ix_kitchen_owner_id ON public.kitchen USING btree (owner_id);
CREATE INDEX ix_kitchen_province_id ON public.kitchen USING btree (province_id);
CREATE INDEX ix_kitchen_ward_id ON public.kitchen USING btree (ward_id);


-- public.kitchen_promotion definition

-- Drop table

-- DROP TABLE public.kitchen_promotion;

CREATE TABLE public.kitchen_promotion (
	kitchens_id uuid NOT NULL,
	promotions_id uuid NOT NULL,
	CONSTRAINT pk_kitchen_promotion PRIMARY KEY (kitchens_id, promotions_id),
	CONSTRAINT fk_kitchen_promotion_kitchen_kitchens_id FOREIGN KEY (kitchens_id) REFERENCES public.kitchen(id) ON DELETE CASCADE,
	CONSTRAINT fk_kitchen_promotion_promotion_promotions_id FOREIGN KEY (promotions_id) REFERENCES public.promotion(id) ON DELETE CASCADE
);
CREATE INDEX ix_kitchen_promotion_promotions_id ON public.kitchen_promotion USING btree (promotions_id);


-- public.notification definition

-- Drop table

-- DROP TABLE public.notification;

CREATE TABLE public.notification (
	id uuid NOT NULL,
	"content" varchar(255) NOT NULL,
	title varchar(100) NOT NULL,
	notification_type text NOT NULL,
	receiver_id uuid NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_notification PRIMARY KEY (id),
	CONSTRAINT fk_notification_user_receiver_id FOREIGN KEY (receiver_id) REFERENCES public."user"(id) ON DELETE CASCADE
);
CREATE INDEX ix_notification_receiver_id ON public.notification USING btree (receiver_id);


-- public."order" definition

-- Drop table

-- DROP TABLE public."order";

CREATE TABLE public."order" (
	id uuid NOT NULL,
	total_price float8 NOT NULL,
	voucher_id uuid NULL,
	customer_id uuid NOT NULL,
	status text NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_order PRIMARY KEY (id),
	CONSTRAINT fk_order_customer_customer_id FOREIGN KEY (customer_id) REFERENCES public.customer(id) ON DELETE CASCADE,
	CONSTRAINT fk_order_voucher_voucher_id FOREIGN KEY (voucher_id) REFERENCES public.voucher(id)
);
CREATE INDEX ix_order_customer_id ON public."order" USING btree (customer_id);
CREATE INDEX ix_order_voucher_id ON public."order" USING btree (voucher_id);


-- public.order_payment definition

-- Drop table

-- DROP TABLE public.order_payment;

CREATE TABLE public.order_payment (
	id uuid NOT NULL,
	"name" varchar(10) NOT NULL,
	order_id uuid NOT NULL,
	status text NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_order_payment PRIMARY KEY (id),
	CONSTRAINT fk_order_payment_order_order_id FOREIGN KEY (order_id) REFERENCES public."order"(id) ON DELETE CASCADE
);
CREATE UNIQUE INDEX ix_order_payment_order_id ON public.order_payment USING btree (order_id);


-- public.conversation definition

-- Drop table

-- DROP TABLE public.conversation;

CREATE TABLE public.conversation (
	id uuid NOT NULL,
	customer_id uuid NOT NULL,
	kitchen_id uuid NOT NULL,
	"content" varchar(500) NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_conversation PRIMARY KEY (id),
	CONSTRAINT fk_conversation_customer_customer_id FOREIGN KEY (customer_id) REFERENCES public.customer(id) ON DELETE CASCADE,
	CONSTRAINT fk_conversation_kitchen_kitchen_id FOREIGN KEY (kitchen_id) REFERENCES public.kitchen(id) ON DELETE CASCADE
);
CREATE INDEX ix_conversation_customer_id ON public.conversation USING btree (customer_id);
CREATE INDEX ix_conversation_kitchen_id ON public.conversation USING btree (kitchen_id);


-- public.favourite_kitchen definition

-- Drop table

-- DROP TABLE public.favourite_kitchen;

CREATE TABLE public.favourite_kitchen (
	id uuid NOT NULL,
	customer_id uuid NOT NULL,
	kitchen_id uuid NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_favourite_kitchen PRIMARY KEY (id),
	CONSTRAINT fk_favourite_kitchen_customer_customer_id FOREIGN KEY (customer_id) REFERENCES public.customer(id) ON DELETE CASCADE,
	CONSTRAINT fk_favourite_kitchen_kitchen_kitchen_id FOREIGN KEY (kitchen_id) REFERENCES public.kitchen(id) ON DELETE CASCADE
);
CREATE INDEX ix_favourite_kitchen_customer_id ON public.favourite_kitchen USING btree (customer_id);
CREATE INDEX ix_favourite_kitchen_kitchen_id ON public.favourite_kitchen USING btree (kitchen_id);


-- public.feedbacks definition

-- Drop table

-- DROP TABLE public.feedbacks;

CREATE TABLE public.feedbacks (
	id uuid NOT NULL,
	customer_id uuid NOT NULL,
	kitchen_id uuid NOT NULL,
	"content" varchar(500) NOT NULL,
	rating int4 NOT NULL,
	created_date timestamptz NOT NULL,
	updated_date timestamptz NOT NULL,
	created_by text NULL,
	updated_by text NULL,
	is_deleted bool NOT NULL,
	CONSTRAINT pk_feedbacks PRIMARY KEY (id),
	CONSTRAINT fk_feedbacks_customer_customer_id FOREIGN KEY (customer_id) REFERENCES public.customer(id) ON DELETE CASCADE,
	CONSTRAINT fk_feedbacks_kitchen_kitchen_id FOREIGN KEY (kitchen_id) REFERENCES public.kitchen(id) ON DELETE CASCADE
);
CREATE INDEX ix_feedbacks_customer_id ON public.feedbacks USING btree (customer_id);
CREATE INDEX ix_feedbacks_kitchen_id ON public.feedbacks USING btree (kitchen_id);
