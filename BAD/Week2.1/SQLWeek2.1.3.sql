USE MappingEx;

--4.	Create a N-N relationship between an order and its items. The same item can occur on multiple orders.

CREATE TABLE COrder(
    order_id int PRIMARY KEY,
    customer_id INT,
    order_date DATE
)

CREATE TABLE Item(
    item_id INT PRIMARY KEY,
    iten_name VARCHAR(100),
    item_price DECIMAL(10,2)
)

-- Junction table for the many-to-many relationship between orders and items

CREATE TABLE COrder_item(
    order_id INT,
    item_id INT,
    PRIMARY KEY (order_id, item_id),
    FOREIGN KEY (order_id) REFERENCES COrder (order_id),
    FOREIGN KEY (item_id) REFERENCES Item (item_id)
)