DO $$
    BEGIN
        -- Check if the table 'message_log' exists
        IF EXISTS (
            SELECT 1
            FROM pg_tables
            WHERE schemaname = 'public' AND tablename = 'message_log'
        ) THEN
            -- Check if it is already a hypertable
            IF NOT EXISTS (
                SELECT 1
                FROM timescaledb_information.hypertables
                WHERE hypertable_name = 'message_log'
            ) THEN
                -- Convert to hypertable
                PERFORM create_hypertable('message_log', 'time');
            END IF;
        ELSE
            -- Table does not exist; you can create it and make it a hypertable
            CREATE TABLE message_log (
                                      time TIMESTAMPTZ NOT NULL,
                                      topic text,
                                      message bytea
            );
            PERFORM create_hypertable('message_log', 'time');
        END IF;
    END $$;
