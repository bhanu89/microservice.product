-- Create a new schema called retail if it doesn't exist
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_namespace WHERE nspname = 'retail') THEN
        EXECUTE 'CREATE SCHEMA retail';
    END IF;
END $$;

-- Create the currency table in the retail schema if it doesn't exist
CREATE TABLE IF NOT EXISTS retail.currency (
    Id SERIAL PRIMARY KEY,
    CurrencyCode VARCHAR NOT NULL,
    CurrencySymbol VARCHAR NOT NULL,
    Created TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    LastModified TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Create the product table in the retail schema if it doesn't exist
CREATE TABLE IF NOT EXISTS retail.product (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR NOT NULL,
    Brand VARCHAR NOT NULL,
    Version BIGINT NOT NULL DEFAULT 1,
    Created TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    LastModified TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CurrencyCodeId INT NOT NULL,
    Price DECIMAL NOT NULL,
    Deleted BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT fk_currency FOREIGN KEY (CurrencyCodeId) REFERENCES retail.currency (Id)
);

-- Function to update the LastModified column and increment Version on update for product table
CREATE OR REPLACE FUNCTION retail.update_product_lastmodified()
RETURNS TRIGGER AS $$
BEGIN
    NEW.LastModified = CURRENT_TIMESTAMP;
    NEW.Version = OLD.Version + 1;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger to call the function on update for product table
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_trigger WHERE tgname = 'update_product_lastmodified_trigger') THEN
        CREATE TRIGGER update_product_lastmodified_trigger
        BEFORE UPDATE ON retail.product
        FOR EACH ROW
        EXECUTE FUNCTION retail.update_product_lastmodified();
    END IF;
END $$;

-- Function to update the LastModified column on update for currency table
CREATE OR REPLACE FUNCTION retail.update_currency_lastmodified()
RETURNS TRIGGER AS $$
BEGIN
    NEW.LastModified = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger to call the function on update for currency table
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_trigger WHERE tgname = 'update_currency_lastmodified_trigger') THEN
        CREATE TRIGGER update_currency_lastmodified_trigger
        BEFORE UPDATE ON retail.currency
        FOR EACH ROW
        EXECUTE FUNCTION retail.update_currency_lastmodified();
    END IF;
END $$;

-- INSERT MOCK DATA
insert into retail.currency (currencycode, currencysymbol, created, lastmodified)
values ('USD','$',CURRENT_TIMESTAMP,CURRENT_TIMESTAMP);
insert into retail.product (name, brand, currencycodeid, price, created, lastmodified)
values ('Laptop','Lenovo',1,1000.00,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP);
insert into retail.product (name, brand, currencycodeid, price, created, lastmodified)
values ('Macbook','Apple',1,2000.00,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP);
insert into retail.product (name, brand, currencycodeid, price, deleted, created, lastmodified)
values ('Laptop','Dell',1,1200.00,true,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP);


