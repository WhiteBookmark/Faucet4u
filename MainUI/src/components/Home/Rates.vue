<template>
  <mdb-row>
    <mdb-col col="1">
      <skyscrapper-banner></skyscrapper-banner>
      <br />
      <skyscrapper-banner></skyscrapper-banner>
      <br />
      <skyscrapper-banner></skyscrapper-banner>
    </mdb-col>
    <mdb-col class="text-center">
      <mdb-row>
        <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
        <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
      </mdb-row>

      <mdb-row>
        <mdb-col col="1"></mdb-col>
        <mdb-col>
          <!--Self-->
          <mdb-tbl striped bordered hover small>
            <mdb-tbl-head color="secondary-color text-white">
              <tr>
                <th colspan="2">User Credit</th>
              </tr>
              <tr>
                <th>Type</th>
                <th>Amount</th>
              </tr>
            </mdb-tbl-head>

            <mdb-tbl-body>
              <tr>
                <th>Faucet Claim</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("LinkClaimCredit")
                      .Value
                  }}
                </th>
              </tr>
              <tr>
                <th>Faucet Claim Interval</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("LinkClaimMinutes")
                      .Value
                  }}
                  Minutes
                </th>
              </tr>
              <tr>
                <th>Bonus Ad</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("BonusAdCredit")
                      .Value
                  }}
                </th>
              </tr>
              <tr>
                <th>PTP Unique Visit</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("PTPUniqueIPCredit")
                      .Value
                  }}
                </th>
              </tr>
              <tr>
                <th>PTP 1st Non-Unique Visit</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue(
                      "PTPNonUniqueIP1Credit"
                    ).Value
                  }}
                </th>
              </tr>
              <tr>
                <th>PTP 2nd Non-Unique Visit</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue(
                      "PTPNonUniqueIP2Credit"
                    ).Value
                  }}
                </th>
              </tr>
              <tr>
                <th>PTP 3rd Non-Unique Visit</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue(
                      "PTPNonUniqueIP3Credit"
                    ).Value
                  }}
                </th>
              </tr>
              <tr>
                <th>Direct Unique Visit</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue(
                      "DirectUniqueIPCredit"
                    ).Value
                  }}
                </th>
              </tr>
              <tr>
                <th>Direct 1st Non-Unique Visit</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue(
                      "DirectNonUniqueIP1Credit"
                    ).Value
                  }}
                </th>
              </tr>
              <tr>
                <th>Direct 2nd Non-Unique Visit</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue(
                      "DirectNonUniqueIP2Credit"
                    ).Value
                  }}
                </th>
              </tr>
              <tr>
                <th>Direct 3rd Non-Unique Visit</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue(
                      "DirectNonUniqueIP3Credit"
                    ).Value
                  }}
                </th>
              </tr>
            </mdb-tbl-body>
          </mdb-tbl>
          <!--Referral-->
          <mdb-tbl striped bordered hover small>
            <mdb-tbl-head color="secondary-color text-white">
              <tr>
                <th colspan="4">Referral Credit</th>
              </tr>
              <tr>
                <th>Type</th>
                <th>Level 1</th>
                <th>Level 2</th>
                <th>Level 3</th>
              </tr>
            </mdb-tbl-head>

            <mdb-tbl-body>
              <tr>
                <th>Faucet Claim</th>
                <th>
                  {{ this.$store.getters.faucetSettingValue("Level1").Value }}%
                </th>
                <th>
                  {{ this.$store.getters.faucetSettingValue("Level2").Value }}%
                </th>
                <th>
                  {{ this.$store.getters.faucetSettingValue("Level3").Value }}%
                </th>
              </tr>
              <tr>
                <th>Bonus Ad</th>
                <th>
                  {{ this.$store.getters.faucetSettingValue("Level1").Value }}%
                </th>
                <th>
                  {{ this.$store.getters.faucetSettingValue("Level2").Value }}%
                </th>
                <th>
                  {{ this.$store.getters.faucetSettingValue("Level3").Value }}%
                </th>
              </tr>
              <tr>
                <th>PTP</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("PTPLevel1").Value
                  }}%
                </th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("PTPLevel2").Value
                  }}%
                </th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("PTPLevel3").Value
                  }}%
                </th>
              </tr>
              <tr>
                <th>Direct</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("PTPLevel1").Value
                  }}%
                </th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("PTPLevel2").Value
                  }}%
                </th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("PTPLevel3").Value
                  }}%
                </th>
              </tr>
              <tr>
                <th>Offerwall</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("OfferwallLevel1")
                      .Value
                  }}%
                </th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("OfferwallLevel2")
                      .Value
                  }}%
                </th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("OfferwallLevel3")
                      .Value
                  }}%
                </th>
              </tr>
            </mdb-tbl-body>
          </mdb-tbl>
          <!--Account Type-->
          <mdb-tbl striped bordered hover small>
            <mdb-tbl-head color="secondary-color text-white">
              <tr>
                <th colspan="2">
                  Earning credited in which account ? (Including referrals)
                </th>
              </tr>
              <tr>
                <th>Earning Type</th>
                <th>Credited in Account</th>
              </tr>
            </mdb-tbl-head>

            <mdb-tbl-body>
              <tr>
                <th>Faucet Claim Earning</th>
                <th>Account Balance</th>
              </tr>
              <tr>
                <th>Bonus Ad Earning</th>
                <th>Account Balance</th>
              </tr>
              <tr>
                <th>PTP Earning</th>
                <th>Account Balance</th>
              </tr>
              <tr>
                <th>Direct Earning</th>
                <th>Account Balance</th>
              </tr>
              <tr>
                <th>Offerwall Earning</th>
                <th>Offerwall Balance</th>
              </tr>
            </mdb-tbl-body>
          </mdb-tbl>
          <!--Deposit/Withdrawal-->
          <mdb-tbl striped bordered hover small>
            <mdb-tbl-head color="secondary-color text-white">
              <tr>
                <th colspan="2">Deposit/Withdrawal Info</th>
              </tr>
              <tr>
                <th>Type</th>
                <th>Value</th>
              </tr>
            </mdb-tbl-head>

            <mdb-tbl-body>
              <tr>
                <th>Minimum Deposit</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("MinimumDeposit")
                      .Value
                  }}
                </th>
              </tr>
              <tr>
                <th>Minimum Withdrawal</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue("MinimumWithdrawal")
                      .Value
                  }}<sup>1</sup>
                </th>
              </tr>
              <tr>
                <th>Minimum Offerwall Withdrawal</th>
                <th>
                  {{
                    this.$store.getters.faucetSettingValue(
                      "MinimumOfferwallWithdrawal"
                    ).Value
                  }}<sup>1</sup>
                </th>
              </tr>
              <tr>
                <th>Withdrawal Fee</th>
                <th>0.00000400<sup>2</sup></th>
              </tr>
              <tr>
                <th>Offerwall Withdrawal Fee</th>
                <th>0.00000400<sup>2</sup></th>
              </tr>
              <tr>
                <th>Withdrawal Processing Time</th>
                <th>1-7 Days</th>
              </tr>
              <tr>
                <th>Withdrawal Time Frame</th>
                <th>Unlimited</th>
              </tr>
            </mdb-tbl-body>
          </mdb-tbl>

          <div class="font-weight-bold text-info">
            <sup>1</sup>: Minimum withdrawal for any method other than Bitcoin
            or Faucethub is
            {{
              this.$store.getters.faucetSettingValue(
                "MinimumExchangeWithdrawal"
              ).Value
            }}
            <br />
            <sup>2</sup>: Faucethub withdrawals will always have 0 fee
            <br />
          </div>
        </mdb-col>
        <mdb-col col="1"></mdb-col>
      </mdb-row>
    </mdb-col>
    <mdb-col col="1">
      <skyscrapper-banner></skyscrapper-banner>
      <br />
      <skyscrapper-banner></skyscrapper-banner>
      <br />
      <skyscrapper-banner></skyscrapper-banner>
      <br />
    </mdb-col>
    <mdb-col col="1"> </mdb-col>

    <button
      type="button"
      class="bottomLeftButton"
      id="bottomLeftButton"
      onClick="document.getElementById('bottomLeftBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomLeftBanner" id="bottomLeftBanner">
      <square-banner></square-banner>
    </div>

    <button
      type="button"
      class="bottomRightButton"
      id="bottomRightButton"
      onClick="document.getElementById('bottomRightBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomRightBanner" id="bottomRightBanner">
      <square-banner></square-banner>
    </div>
  </mdb-row>
</template>

<script>
import {
  mdbRow,
  mdbCol,
  Btn,
  mdbTbl,
  mdbTblHead,
  mdbTblBody,
  mdbBtn
} from "mdbvue";

export default {
  components: { mdbRow, mdbCol, Btn, mdbTbl, mdbTblHead, mdbTblBody, mdbBtn },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      address: this.$store.state.userData[0].FaucetHubBitcoinAddress,
      apiResponse: null,
      errorList: null,
      successList: null,
      value: null,
      price: null,
      banner468Values: [],
      banner125Values: [],
      ptpRotatorValues: [],
      ptpDirectValues: [],
      banner468Selected: null,
      banner125Selected: null,
      PTPRotatorSelected: null,
      PTPDirectSelected: null
    };
  },
  methods: {
    test: function() {}
  },
  mounted() {}
};
</script>
