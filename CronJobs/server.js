var cron = require('node-cron');
const sql = require('mssql')
var moment = require('moment');
var request = require('request');
var io = require('socket.io-client');
var socket = io.connect('http://127.0.0.1:3000', { reconnect: false });

const paymentKey = "OyDSf6q7wx%m";
const overrideValidation = "456a9477-e685-4ce9-b036-5819e8e2c468";
const particularOutdatedSocketKey = "P46$SP*V7Qrz";

const config = {
    user: 'sa',
    password: '123',
    server: 'localhost',
    database: 'Faucet4u'
}

const config2 = {
    user: 'sa',
    password: '*7^Ut!Um#g41',
    server: '66.206.39.103',
    database: 'Faucet4u'
}

const config3 = {
    user: 'sa',
    password: '*7^Ut!Um#g41',
    server: '66.206.39.103',
    database: 'AdminPanel'
}

//Note: To reduce server load, some tasks have been distributed into 2 or more crons, 1st one runs every minute for critical updates
//While other(s) run once in x hours/days since their updation is not so important

//Banner rotator running every minute to change target and image link
cron.schedule('* * * * *', () => {
    new sql.ConnectionPool(config).connect().then(pool => {
        return pool.request()
            .query(`BEGIN                    
                    EXEC UpdateBannerRotatorCounter
                    END`)
    }).then(result => {
        sql.close();

    }).catch(err => {
        console.log(err);
        sql.close();
    })
    sql.on('error', err => {
        console.log(err)
        sql.close();
    })
});

//Square Banner rotator running every minute to change target and image link
cron.schedule('* * * * *', () => {
    new sql.ConnectionPool(config).connect().then(pool => {
        return pool.request()
            .query(`BEGIN
                    EXEC UpdateSquareBannerRotatorCounter
                    END`)
    }).then(result => {
        sql.close();

    }).catch(err => {
        console.log(err);
        sql.close();
    })
    sql.on('error', err => {
        console.log(err)
        sql.close();
    })
});

//Flush the data of PTP IP, Direct IP, BonusAdsRecord and LinkShortnersRecord at every 24 hours
cron.schedule('1 23 * * *', () => {
    new sql.ConnectionPool(config).connect().then(pool => {
        return pool.request()
            .query(`BEGIN

                    DELETE FROM PTPIP
                    DELETE FROM DirectIP
                    DELETE FROM LinkShortnersRecord
                    DELETE FROM BonusAdsRecord

                    END`)
    }).then(result => {
        sql.close();

    }).catch(err => {
        console.log(err);
        sql.close();
    })
    sql.on('error', err => {
        console.log(err)
        sql.close();
    })
});

//Check and update the confirmations in database if a new deposit invoice has been paid (Every minute)
cron.schedule('* * * * *', () => {
    new sql.ConnectionPool(config2).connect().then(pool => {
        return pool.request()
            .query(`BEGIN
                    SELECT * FROM DepositHistory WHERE TransactionId = 'Unknown' AND Confirmations <= 6
                    END`)
    }).then(result => {


        for (var i = 0; i < result.recordset.length; i++) {

            var data = result.recordset[i];

            request.get(
                'http://api.faucet4all.com/Payment/GetRequest',
                {
                    qs: {
                        apiKey: paymentKey,
                        address: result.recordset[i].WalletAddress,
                        sessionId: overrideValidation
                    }
                },
                function (error, response, body) {

                    if (response.statusCode !== 200) {

                        console.log(error);
                        console.log(response);
                        console.log(body);
                        console.log("Response code isn't 200");
                    }
                    else if (data.InvoiceId === JSON.parse(body).result.id) {

                        if (JSON.parse(body).result.hasOwnProperty('confirmations')) {

                            if (JSON.parse(body).result.confirmations > data.Confirmations) {

                                var addAmount = false;

                                if (data.Confirmations < 6 && JSON.parse(body).result.confirmations >= 6) {
                                    addAmount = true;
                                }

                                new sql.ConnectionPool(config2).connect().then(pool => {
                                    return pool.request()
                                        .input("Confirmations", JSON.parse(body).result.confirmations)
                                        .input("InvoiceId", JSON.parse(body).result.id)
                                        .input("Add", addAmount)
                                        .query(`
                                   BEGIN	
                                   DECLARE @Username varchar(10)                                   	
                                   SELECT @Username = Username FROM DepositHistory WHERE InvoiceId = @InvoiceId
                                   UPDATE DepositHistory SET Confirmations = @Confirmations WHERE InvoiceId = @InvoiceId	
                                   IF(@Add = 1)
                                   BEGIN
                                   DECLARE @DepositAmount decimal(9, 8) = 0.00000000
                                   SELECT @DepositAmount = Amount FROM DepositHistory WHERE InvoiceId = @InvoiceId
                                   UPDATE Users SET PurchaseBalance += @DepositAmount WHERE Username = @Username
                                   END	
                                   SELECT SocketSession FROM Users WHERE Username = @Username                                   
                                   END`)
                                }).then(subResult => {

                                    socket.emit('updateParticularUserData', [{ "Key": particularOutdatedSocketKey, "SocketSession": subResult.recordset[0].SocketSession }]);

                                }).catch(err => {

                                    console.log(err);
                                    sql.close();
                                })
                                sql.on('error', err => {
                                    console.log(err)
                                    sql.close();
                                })
                            }
                        }
                    }


                });
        }

        sql.close();

    }).catch(err => {
        console.log(err);
        sql.close();
    })
    sql.on('error', err => {
        console.log(err)
        sql.close();
    })
});

//Check and update the confirmations in database for old deposits, once a day
cron.schedule('1 23 * * *', () => {
    new sql.ConnectionPool(config2).connect().then(pool => {
        return pool.request()
            .query(`BEGIN
                    SELECT * FROM DepositHistory WHERE Confirmations > 6
                    END`)
    }).then(result => {


        for (var i = 0; i < result.recordset.length; i++) {

            var data = result.recordset[i];

            request.get(
                'http://api.faucet4all.com/Payment/GetRequest',
                {
                    qs: {
                        apiKey: paymentKey,
                        address: result.recordset[i].WalletAddress,
                        sessionId: overrideValidation
                    }
                },
                function (error, response, body) {

                    if (response.statusCode !== 200) {

                        console.log(error);
                        console.log(response);
                        console.log(body);
                        console.log("Response code isn't 200");
                    }
                    else if (data.InvoiceId === JSON.parse(body).result.id) {

                        if (JSON.parse(body).result.hasOwnProperty('confirmations')) {

                            if (JSON.parse(body).result.confirmations > data.Confirmations) {

                                var addAmount = false;

                                if (data.Confirmations < 6 && JSON.parse(body).result.confirmations >= 6) {
                                    addAmount = true;
                                }

                                new sql.ConnectionPool(config2).connect().then(pool => {
                                    return pool.request()
                                        .input("Confirmations", JSON.parse(body).result.confirmations)
                                        .input("InvoiceId", JSON.parse(body).result.id)
                                        .input("Add", addAmount)
                                        .query(`
                                   BEGIN	
                                   DECLARE @Username varchar(10)                                   	
                                   SELECT @Username = Username FROM DepositHistory WHERE InvoiceId = @InvoiceId
                                   UPDATE DepositHistory SET Confirmations = @Confirmations WHERE InvoiceId = @InvoiceId	
                                   IF(@Add = 1)
                                   BEGIN
                                   DECLARE @DepositAmount decimal(9, 8) = 0.00000000
                                   SELECT @DepositAmount = Amount FROM DepositHistory WHERE InvoiceId = @InvoiceId
                                   UPDATE Users SET PurchaseBalance += @DepositAmount WHERE Username = @Username
                                   END	
                                   SELECT SocketSession FROM Users WHERE Username = @Username                                   
                                   END`)
                                }).then(subResult => {

                                    socket.emit('updateParticularUserData', [{ "Key": particularOutdatedSocketKey, "SocketSession": subResult.recordset[0].SocketSession }]);

                                }).catch(err => {

                                    console.log(err);
                                    sql.close();
                                })
                                sql.on('error', err => {
                                    console.log(err)
                                    sql.close();
                                })
                            }
                        }
                    }


                });
        }

        sql.close();

    }).catch(err => {
        console.log(err);
        sql.close();
    })
    sql.on('error', err => {
        console.log(err)
        sql.close();
    })
});

//Delete spam records once every 10 days which has 0 confirmations since 10 days from their creation
cron.schedule('1 23 10,20,30 * *', () => {
    new sql.ConnectionPool(config).connect().then(pool => {
        return pool.request()
            .query(`BEGIN

                    DELETE FROM DepositHistory WHERE DepositDate < DATEADD(day, -10, GETDATE()) AND Confirmations = 0

                    END`)
    }).then(result => {
        sql.close();

    }).catch(err => {
        console.log(err);
        sql.close();
    })
    sql.on('error', err => {
        console.log(err)
        sql.close();
    })
});

//Check and update the confirmations in admin panel withdrawals every minute
cron.schedule('* * * * *', () => {
    new sql.ConnectionPool(config3).connect().then(pool => {
        return pool.request()
            .query(`BEGIN
                    SELECT * FROM WithdrawalHistory WHERE Confirmations <= 6
                    END`)
    }).then(result => {


        for (var i = 0; i < result.recordset.length; i++) {

            var data = result.recordset[i];

            request.get(
                'http://api.faucet4all.com/Payment/GetConfirmation',
                {
                    qs: {
                        apiKey: paymentKey,
                        address: result.recordset[i].Transaction,
                        sessionId: overrideValidation
                    }
                },
                function (error, response, body) {

                    if (response.statusCode !== 200) {

                        console.log(error);
                        console.log(response);
                        console.log(body);
                        console.log("Response code isn't 200");
                    }
                    else if (JSON.parse(body).result.hasOwnProperty('confirmations')) {

                        if (JSON.parse(body).result.confirmations > data.Confirmations) {

                            new sql.ConnectionPool(config3).connect().then(pool => {
                                return pool.request()
                                    .input("Confirmations", JSON.parse(body).result.confirmations)
                                    .input("Transaction", result.recordset[i].Transaction)
                                    .query(`
                                   BEGIN	
                                   UPDATE WithdrawalHistory SET Confirmations += @Confirmations WHERE Transaction = @Transaction                            
                                   END`)
                            }).then(subResult => {


                            }).catch(err => {

                                console.log(err);
                                sql.close();
                            })
                            sql.on('error', err => {
                                console.log(err)
                                sql.close();
                            })
                        }
                    }
                });
        }

        sql.close();

    }).catch(err => {
        console.log(err);
        sql.close();
    })
    sql.on('error', err => {
        console.log(err)
        sql.close();
    })
});

//Check and update the confirmations in admin panel for old withdrawals once a day
cron.schedule('1 23 * * *', () => {
    new sql.ConnectionPool(config3).connect().then(pool => {
        return pool.request()
            .query(`BEGIN
                    SELECT * FROM WithdrawalHistory WHERE Confirmations > 6
                    END`)
    }).then(result => {


        for (var i = 0; i < result.recordset.length; i++) {

            var data = result.recordset[i];

            request.get(
                'http://api.faucet4all.com/Payment/GetConfirmation',
                {
                    qs: {
                        apiKey: paymentKey,
                        address: result.recordset[i].Transaction,
                        sessionId: overrideValidation
                    }
                },
                function (error, response, body) {

                    if (response.statusCode !== 200) {

                        console.log(error);
                        console.log(response);
                        console.log(body);
                        console.log("Response code isn't 200");
                    }
                    else if (JSON.parse(body).result.hasOwnProperty('confirmations')) {

                        if (JSON.parse(body).result.confirmations > data.Confirmations) {

                            new sql.ConnectionPool(config3).connect().then(pool => {
                                return pool.request()
                                    .input("Confirmations", JSON.parse(body).result.confirmations)
                                    .input("Transaction", result.recordset[i].Transaction)
                                    .query(`
                                   BEGIN	
                                   UPDATE WithdrawalHistory SET Confirmations += @Confirmations WHERE Transaction = @Transaction                            
                                   END`)
                            }).then(subResult => {


                            }).catch(err => {

                                console.log(err);
                                sql.close();
                            })
                            sql.on('error', err => {
                                console.log(err)
                                sql.close();
                            })
                        }
                    }
                });
        }

        sql.close();

    }).catch(err => {
        console.log(err);
        sql.close();
    })
    sql.on('error', err => {
        console.log(err)
        sql.close();
    })
});

console.log('Cron job started ' + moment(Date.now()).format('YYYY-MM-DD - hh:mm:ss A'));