docker run --rm -p 3306:3306 --env "MYSQL_ALLOW_EMPTY_PASSWORD=yes" --env "MYSQL_DATABASE=test"  -d percona:5.7.20 --character-set-server=utf8mb4 --collation-server=utf8mb4_unicode_ci
