CREATE TABLE IF NOT EXISTS tb_item (
    id BIGSERIAL PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    description TEXT,
    category VARCHAR(100),
    found_location VARCHAR(200),
    found_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(20) NOT NULL DEFAULT 'ENCONTRADO',
    person_who_found VARCHAR(150),
    contact_who_found VARCHAR(150),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT chk_tb_item_status
        CHECK (status IN ('PERDIDO', 'ENCONTRADO', 'DEVOLVIDO'))
);
