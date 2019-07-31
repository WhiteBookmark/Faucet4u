<template>
    <mdb-container fluid>
        <mdb-row>
            <mdb-col>
                <standard-banner></standard-banner>
            </mdb-col>
            <mdb-col>
                <standard-banner></standard-banner>
            </mdb-col>
        </mdb-row>

        <br />
        <br />

        <mdb-row>
            <mdb-col col="1"></mdb-col>
            <mdb-col>
                <mdb-card border="primary">
                    <mdb-card-body class="text-dark text-center">
                        <mdb-card-title tag="h1">Welcome To Faucet4u!</mdb-card-title>
                        <mdb-card-text>
                            <p>
                                A free Bitcoin earning and Advertising site where you can earn
                                massive. Over 15 Ways to Earn Bitcoins!!<br />
                                Multiple referral levels and PTP earnings for all levels !<br />
                                Advertising as low as 5 satoshi per view !!<br />
                                Withdraw money directly to your wallet or faucethub !!!
                            </p>
                        </mdb-card-text>
                    </mdb-card-body>
                </mdb-card>
                <br />
            </mdb-col>
            <mdb-col col="1"></mdb-col>
        </mdb-row>

        <mdb-row>
            <mdb-col>
                <standard-banner></standard-banner>
            </mdb-col>
            <mdb-col>
                <standard-banner></standard-banner>
            </mdb-col>
        </mdb-row>

        <br />
        <br />

        <mdb-row>
            <mdb-col col="1"></mdb-col>
            <mdb-col class="text-center">
                <mdb-card>
                    <mdb-card-header color="primary-color" tag="h3" class="text-center">Members</mdb-card-header>
                    <mdb-card-body class="text-center">
                        <mdb-card-text>
                            <ul class="list-unstyled">
                                <li>Earn Bitcoins in 10+ ways</li>
                                <li>Payments within 48 Hours!</li>
                                <li>0.0002 BTC Min Cashout!</li>
                                <li>Massive Affiliate Program!</li>
                            </ul>
                        </mdb-card-text>
                        <mdb-btn color="primary" v-on:click="register()">Members Registeration</mdb-btn>
                    </mdb-card-body>
                </mdb-card>
            </mdb-col>
            <mdb-col col="1"></mdb-col>

            <mdb-col class="text-center">
                <mdb-card>
                    <mdb-card-header color="primary-color" tag="h3" class="text-center">Advertisers</mdb-card-header>
                    <mdb-card-body class="text-center">
                        <mdb-card-text>
                            <ul class="list-unstyled">
                                <li>Only Crypto Targeted Audience!</li>
                                <li>CPC starts at only 5 satoshis per click!</li>
                                <li>Ads loads without a frame!</li>
                                <li>GUARANTEED Results!</li>
                            </ul>
                        </mdb-card-text>
                        <mdb-btn color="primary" v-on:click="register()">Advertisers Registeration</mdb-btn>
                    </mdb-card-body>
                </mdb-card>
            </mdb-col>
            <mdb-col col="1"></mdb-col>
        </mdb-row>

        <br />
        <mdb-row>
            <mdb-col>
                <standard-banner></standard-banner>
            </mdb-col>
            <mdb-col>
                <standard-banner></standard-banner>
            </mdb-col>
        </mdb-row>

    </mdb-container>
</template>

<script>
    import {
        mdbCol,
        mdbRow,
        mdbContainer,
        mdbBtn,
        mdbCard,
        mdbCardHeader,
        mdbCardBody,
        mdbCardTitle,
        mdbCardText
    } from "mdbvue";

    export default {
        components: {
            mdbCol,
            mdbRow,
            mdbContainer,
            mdbBtn,
            mdbCard,
            mdbCardHeader,
            mdbCardBody,
            mdbCardTitle,
            mdbCardText
        },
        methods: {
            test: function () { },
            register: function () {
                this.$router.push("AccountRegister");
            }
        },
        data() {
            return {};
        },
        mounted() { }
    };
</script>
