-- FUNCTION: public.het_rntrrl_ar_iud_tr()

--DROP FUNCTION public.het_rntrrl_ar_iud_tr();

CREATE FUNCTION public.het_rntrrl_ar_iud_tr()
    RETURNS trigger
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE NOT LEAKPROOF 
    COST 100
    ROWS 0
AS $BODY$

	DECLARE
        l_current_timestamp timestamp;
	BEGIN
        l_current_timestamp := current_timestamp;
    	IF(TG_OP = 'INSERT') THEN
        	INSERT INTO public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST"
            (
			 "RENTAL_REQUEST_ROTATION_LIST_HIST_ID",
             "RENTAL_REQUEST_ROTATION_LIST_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "ASKED_DATE_TIME",
			 "EQUIPMENT_ID",
			 "IS_FORCE_HIRE",
			 "NOTE",
			 "OFFER_RESPONSE",
			 "OFFER_RESPONSE_NOTE",
			 "RENTAL_AGREEMENT_ID",
			 "RENTAL_REQUEST_ID",
			 "ROTATION_LIST_SORT_ORDER",
			 "WAS_ASKED",
			 "OFFER_REFUSAL_REASON",
			 "OFFER_RESPONSE_DATETIME",
			 "CONCURRENCY_CONTROL_NUMBER",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "APP_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_GUID",
			 "APP_CREATE_USERID",
			 "APP_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_GUID",
			 "APP_LAST_UPDATE_USERID",
			 "DB_CREATE_USER_ID",
			 "DB_LAST_UPDATE_USER_ID"
            )
            VALUES( 
              nextval('"HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq"'::regclass),
              NEW."RENTAL_REQUEST_ROTATION_LIST_ID",
              l_current_timestamp,
              NULL,
			  NEW."ASKED_DATE_TIME",
			  NEW."EQUIPMENT_ID",
			  NEW."IS_FORCE_HIRE",
			  NEW."NOTE",
			  NEW."OFFER_RESPONSE",
			  NEW."OFFER_RESPONSE_NOTE",
			  NEW."RENTAL_AGREEMENT_ID",
			  NEW."RENTAL_REQUEST_ID",
			  NEW."ROTATION_LIST_SORT_ORDER",
			  NEW."WAS_ASKED",
			  NEW."OFFER_REFUSAL_REASON",
			  NEW."OFFER_RESPONSE_DATETIME",
			  NEW."CONCURRENCY_CONTROL_NUMBER",
			  NEW."DB_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_DIRECTORY",
			  NEW."DB_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."APP_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_GUID",
			  NEW."APP_CREATE_USERID",
			  NEW."APP_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_GUID",
			  NEW."APP_LAST_UPDATE_USERID",
			  NEW."DB_CREATE_USER_ID",
			  NEW."DB_LAST_UPDATE_USER_ID"
             );
             RETURN NEW;
        ELSIF (TG_OP = 'UPDATE') THEN
        	---- First update the previously active row
            UPDATE public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST" rntrrlhis
            SET "END_DATE" = l_current_timestamp
            WHERE rntrrlhis."RENTAL_REQUEST_ROTATION_LIST_ID" = NEW."RENTAL_REQUEST_ROTATION_LIST_ID"
            AND rntrrlhis."END_DATE" IS NULL;
            ---- Now insert the new current row
        	INSERT INTO public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST"
            (
			 "RENTAL_REQUEST_ROTATION_LIST_HIST_ID",
             "RENTAL_REQUEST_ROTATION_LIST_ID",
             "EFFECTIVE_DATE",
             "END_DATE",
			 "ASKED_DATE_TIME",
			 "EQUIPMENT_ID",
			 "IS_FORCE_HIRE",
			 "NOTE",
			 "OFFER_RESPONSE",
			 "OFFER_RESPONSE_NOTE",
			 "RENTAL_AGREEMENT_ID",
			 "RENTAL_REQUEST_ID",
			 "ROTATION_LIST_SORT_ORDER",
			 "WAS_ASKED",
			 "OFFER_REFUSAL_REASON",
			 "OFFER_RESPONSE_DATETIME",
			 "CONCURRENCY_CONTROL_NUMBER",
			 "DB_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_DIRECTORY",
			 "DB_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_DIRECTORY",
			 "APP_CREATE_TIMESTAMP",
			 "APP_CREATE_USER_GUID",
			 "APP_CREATE_USERID",
			 "APP_LAST_UPDATE_TIMESTAMP",
			 "APP_LAST_UPDATE_USER_GUID",
			 "APP_LAST_UPDATE_USERID",
			 "DB_CREATE_USER_ID",
			 "DB_LAST_UPDATE_USER_ID"
            )
            VALUES( 
              nextval('"HET_RENTAL_REQUEST_ROTATION_LIST_HIST_ID_seq"'::regclass),
              NEW."RENTAL_REQUEST_ROTATION_LIST_ID",
              l_current_timestamp,
              NULL,
			  NEW."ASKED_DATE_TIME",
			  NEW."EQUIPMENT_ID",
			  NEW."IS_FORCE_HIRE",
			  NEW."NOTE",
			  NEW."OFFER_RESPONSE",
			  NEW."OFFER_RESPONSE_NOTE",
			  NEW."RENTAL_AGREEMENT_ID",
			  NEW."RENTAL_REQUEST_ID",
			  NEW."ROTATION_LIST_SORT_ORDER",
			  NEW."WAS_ASKED",
			  NEW."OFFER_REFUSAL_REASON",
			  NEW."OFFER_RESPONSE_DATETIME",
			  NEW."CONCURRENCY_CONTROL_NUMBER",
			  NEW."DB_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_DIRECTORY",
			  NEW."DB_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_DIRECTORY",
			  NEW."APP_CREATE_TIMESTAMP",
			  NEW."APP_CREATE_USER_GUID",
			  NEW."APP_CREATE_USERID",
			  NEW."APP_LAST_UPDATE_TIMESTAMP",
			  NEW."APP_LAST_UPDATE_USER_GUID",
			  NEW."APP_LAST_UPDATE_USERID",
			  NEW."DB_CREATE_USER_ID",
			  NEW."DB_LAST_UPDATE_USER_ID"
             );
     	RETURN NEW;
        ELSIF (TG_OP = 'DELETE') THEN
        	---- First update the previously active row
            UPDATE public."HET_RENTAL_REQUEST_ROTATION_LIST_HIST" rntrrlhis
            SET "END_DATE" = l_current_timestamp
            WHERE rntrrlhis."RENTAL_REQUEST_ROTATION_LIST_ID" = OLD."RENTAL_REQUEST_ROTATION_LIST_ID"
            AND rntrrlhis."END_DATE" IS NULL;
            RETURN NEW;
    END IF;
    RETURN NULL;
   	END;

$BODY$;

--ALTER FUNCTION public.het_rntrrl_ar_iud_tr()
--    OWNER TO "user6DA";

