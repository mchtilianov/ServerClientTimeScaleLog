DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_extension WHERE extname = 'timescaledb') THEN
        CREATE EXTENSION IF NOT EXISTS timescaledb;
END IF;
END $$;
